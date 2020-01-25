<template>
	<SunPage class="sessions">
		<PageHeader :title="$tl('title')" class="page-padding" />
		<q-markup-table v-if="sessions">
			<thead>
				<tr>
					<th>
						<q-checkbox v-model="all" @input="e => allChecked(e)" />
					</th>
					<th class="text-left">
						<q-icon left :name="$iconsSet.Sessions.machine" />
						{{ $tl("deviceInfo") }}
					</th>
					<th class="text-left">
						<q-icon left :name="$iconsSet.Sessions.clock" />
						{{ $tl("updateDate") }}
					</th>
				</tr>
			</thead>
			<tbody>
				<tr v-for="session of sessions">
					<td class="text-center">
						<q-checkbox
							:toggle-indeterminate="false"
							v-if="!session.isCurrent"
							v-model="session.selected"
						/>
						<q-badge class="q-ml-sm" v-else>{{ $tl("current") }}</q-badge>
					</td>
					<td>
						{{ session.deviceInfo }}
					</td>
					<td>
						{{ $formatDate(session.updateDate) }}
					</td>
				</tr>
			</tbody>
		</q-markup-table>

		<LoaderWait v-else />

		<div class="text-center">
			<q-btn
				class="q-mt-md delete-btn"
				:icon="$iconsSet.Sessions.signOut"
				:label="$tl('logout')"
				@click="deleteSessions"
				v-if="selected.length"
				no-caps
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "Sessions",
	mixins: [Page],
	data() {
		return {
			sessions: null
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		},
		all() {
			let tr = 0;
			let fl = 0;
			for (const session of this.sessions) {
				if (!session.isCurrent) {
					if (session.selected) tr++;
					else fl++;
				}
			}
			if (tr > 0 && fl > 0) return null;
			else if (tr > 0) return true;
			else return false;
		},
		selected() {
			return this.sessions.filter(x => x.selected);
		}
	},
	methods: {
		allChecked(e) {
			if (e) {
				for (const session of this.sessions) {
					if (!session.isCurrent) session.selected = true;
				}
			} else {
				for (const session of this.sessions) {
					session.selected = false;
				}
			}
		},
		deleteSessions() {
			let sessions = this.selected.filter(x => !x.isCurrent);
			if (sessions.length === 0) return;

			this.$request(this.$Api.Personal.RemoveMySessions, {
				sessions: sessions.map(x => x.id).join(",")
			}).then(response => {
				this.$successNotify(null, "warning");
				this.selected = [];
				this.loadData();
			});
		},
		loadData() {
			this.$request(this.$Api.Personal.GetMySessions).then(response => {
				for (const session of response.data) session.selected = false;

				this.sessions = response.data;
			});
		}
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss">
.sessions__current {
	background-color: #efffec;
}
</style>
